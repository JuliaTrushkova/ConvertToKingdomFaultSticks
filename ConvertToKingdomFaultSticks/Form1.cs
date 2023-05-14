using System.Security.Cryptography.X509Certificates;

namespace ConvertToKingdomFaultSticks
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string[] Filenames = new string[] { };
        List<FaultsForOneFile> faultsForAllFiles = new List<FaultsForOneFile>();

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                Filenames = openFileDialog1.FileNames;
                for (int i = 0; i < Filenames.Length; i++)
                {
                    StreamReader streamReader = new StreamReader(Filenames[i]);

                    string? line = streamReader.ReadLine();

                    FaultsForOneFile faultsForOneFile = new FaultsForOneFile()
                    {
                        FileName = Filenames[i]
                    };

                    Fault fault = Fault.CreateNewFault(line?.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                    int numberOfStick = 1;
                    while (streamReader.ReadLine() is string nextLine)
                    {
                        string[] lineSplit = nextLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            if (lineSplit[0] == fault.Name)
                            {
                                if (lineSplit[1] != "1")
                                {
                                    fault.FaultSticks.Last().AddFaultStickPoint(lineSplit);
                                }
                                else
                                {
                                    ++numberOfStick;
                                    fault.AddNewFaultStick(lineSplit, numberOfStick);
                                }
                            }
                            else
                            {
                                faultsForOneFile.Faults.Add(fault);
                                Fault? faultFind = faultsForOneFile.Faults.Find(x => x.Name == lineSplit[0]);
                                if (faultFind is null)
                                {
                                    numberOfStick = 1;
                                    fault = Fault.CreateNewFault(lineSplit);
                                }
                                else
                                {
                                    int lastNumberOfStick = faultFind.FaultSticks.Last().NumberOfStick;
                                    FaultStick? faultStickFind = faultFind.FaultSticks.Find(x => x.NameOfProfile == lineSplit[3]);
                                    if (faultStickFind is null)
                                    {
                                        ++lastNumberOfStick;
                                    }
                                    else 
                                    { 
                                        numberOfStick = faultStickFind.NumberOfStick; 
                                    }
                                    faultFind.AddNewFaultStick(lineSplit, lastNumberOfStick);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    faultsForAllFiles.Add(faultsForOneFile);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (faultsForAllFiles is null) { return; }

            foreach (FaultsForOneFile faultsForOneFile in faultsForAllFiles)
            {
                WriteToFileAsync(faultsForOneFile); 
            }
        }

        private static void WriteToFileAsync(FaultsForOneFile faultsForOneFile)
        {
            if (faultsForOneFile.Faults is null || faultsForOneFile.FileName is null) return;

            string? fileName = Path.GetFileNameWithoutExtension(faultsForOneFile.FileName);
            fileName += "_izm";
            string? path = Path.GetDirectoryName(faultsForOneFile.FileName);
            string fullPath = Path.Combine(path, fileName);

            StreamWriter streamWriter = new StreamWriter(fullPath);            

            for (int i = 0; i <  faultsForOneFile.Faults.Count; i++)
            {
                streamWriter.WriteLine(faultsForOneFile.Faults[i].ToString());
            }

            streamWriter.Close();
            streamWriter.Dispose();
        }
    }
}