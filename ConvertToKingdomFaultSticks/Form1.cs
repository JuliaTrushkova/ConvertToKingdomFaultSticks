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

        //��������� ��������� ���� � ��������� ����� ����������� ������ �������� Fault, � �������� ���� ���� ����� ������.
        //������ ���� ������� �� ��������� ����� � ������������� ����������� ������ �������.
        private void button1_Click(object sender, EventArgs e)
        {
            FilePattern filePattern = this.FillFilePattern();
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

                    Fault fault = Fault.CreateNewFault(line?.Split(' ', StringSplitOptions.RemoveEmptyEntries), filePattern);
                    int numberOfStick = 1;
                    while (streamReader.ReadLine() is string nextLine)
                    {
                        string[] lineSplit = nextLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            //���� �������� ������� ������� � ���������� ��������, �� ��������� ������ � ���� ������
                            //���� �� �������, �� ���� �� ���� ��������� ��������. ���� �� �������, �� ��������� ����� ������
                            if (lineSplit[filePattern.ColumnNameOfFault] == fault.Name)
                            {
                                if (lineSplit[filePattern.ColumnPoint] != "1")
                                {
                                    fault.FaultSticks.Last().AddFaultStickPoint(lineSplit, filePattern);
                                }
                                else
                                {
                                    ++numberOfStick;
                                    fault.AddNewFaultStick(lineSplit, numberOfStick, filePattern);
                                }
                            }
                            else
                            {
                                faultsForOneFile.Faults.Add(fault);

                                //���� � ��������� �������� ������ � ����� �� ���������
                                //���� ����� ������ ��� ����, �� ��������� � ���� ����� ������
                                //���� ������ ������� ���, �� ������� ����� ������
                                Fault? faultFind = faultsForOneFile.Faults.Find(x => x.Name == lineSplit[filePattern.ColumnNameOfFault]);
                                if (faultFind is null)
                                {
                                    numberOfStick = 1;
                                    fault = Fault.CreateNewFault(lineSplit, filePattern);
                                }
                                else
                                {
                                    //���� � ������� ���� � ����� �� ��������� �������
                                    //���� � ������� ��� ���� ���� � �������� �������, �� ��������� ����� � ���� ����
                                    //���� ������ ������� ���, �� ������� ����� ����
                                    int lastNumberOfStick = faultFind.FaultSticks.Last().NumberOfStick;
                                    FaultStick? faultStickFind = faultFind.FaultSticks.Find(x => x.NameOfProfile == lineSplit[filePattern.ColumnNameOfProfile]);
                                    if (faultStickFind is null)
                                    {
                                        ++lastNumberOfStick;
                                        faultFind.AddNewFaultStick(lineSplit, lastNumberOfStick, filePattern);
                                    }
                                    else
                                    {                                        
                                        faultStickFind.AddFaultStickPoint(lineSplit, filePattern);
                                    }
                                    
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

        //������ � �����.  
        private void button2_Click(object sender, EventArgs e)
        {
            if (faultsForAllFiles is null) { return; }

            foreach (FaultsForOneFile faultsForOneFile in faultsForAllFiles)
            {
                WriteToFile(faultsForOneFile);
            }
        }

        //������ � ����.��������� �� ����� ������ �������� � ���������� � ������ �������
        private static void WriteToFile(FaultsForOneFile faultsForOneFile)
        {
            if (faultsForOneFile.Faults is null || faultsForOneFile.FileName is null) return;

            string? fileName = Path.GetFileNameWithoutExtension(faultsForOneFile.FileName);
            fileName += "_izm";
            string? path = Path.GetDirectoryName(faultsForOneFile.FileName);
            string fullPath = Path.Combine(path, fileName);

            StreamWriter streamWriter = new StreamWriter(fullPath);

            for (int i = 0; i < faultsForOneFile.Faults.Count; i++)
            {
                streamWriter.WriteLine(faultsForOneFile.Faults[i].ToString());
            }

            streamWriter.Close();
            streamWriter.Dispose();
        }

        private FilePattern FillFilePattern()
        {
            FilePattern filePattern = new FilePattern()
            {
                ColumnX = Convert.ToInt32(this.numericUpDown1.Value),
                ColumnY = Convert.ToInt32(this.numericUpDown2.Value),
                ColumnTime = Convert.ToInt32(this.numericUpDown3.Value),
                ColumnCdp = Convert.ToInt32(this.numericUpDown4.Value),
                ColumnNameOfProfile = Convert.ToInt32(this.numericUpDown5.Value),
                ColumnNameOfFault = Convert.ToInt32(this.numericUpDown6.Value),
                ColumnPoint = Convert.ToInt32(this.numericUpDown7.Value)
            };
            return filePattern;
        }
    }
}