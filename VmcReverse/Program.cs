namespace VmcReverse
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read CSV File
            //const string csvFileName = "2020-01-19_16-46-45_VmcManualVel0.csv";
            const string csvFileName = "2020-01-19_17-23-01_VmcAutoVel2_40sec.csv";
            var data = CsvReader.ReadCsvData(csvFileName);

            var messages = Modbus.ExtractMessages(data);
        }

        
    }
}
