using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using PrinterAccounter.DTOs.Input;
using PrinterAccounter.Exceptions;

namespace PrinterAccounter.Utils;

internal class PrinterCsvParser
{
    public List<PrintTaskDto> ParseTasksCsv(byte[] fileContent)
    {
        if (!IsUtf8Encoded(fileContent))
            throw new FileValidationException("Invalid file encoding");
        try
        {
            using var memoryStream = new MemoryStream(fileContent);
            using var streamReader = new StreamReader(memoryStream, Encoding.UTF8);
            using var csvReader = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            });

            var records = new List<PrintTaskDto>();
            var rowCount = 0;

            while (csvReader.Read())
            {
                if (rowCount >= 100)
                    break;

                var record = new PrintTaskDto
                {
                    Name = csvReader.GetField<string>(0),
                    EmployeeId = csvReader.GetField<int>(1),
                    SerialNumber = csvReader.GetField<int?>(2),
                    PagesCount = csvReader.GetField<int>(3)
                };

                // Validate mandatory fields
                if (!string.IsNullOrEmpty(record.Name) &&
                    record.PagesCount > 0)
                {
                    records.Add(record);
                    rowCount++;
                }
            }

            return records;
        }
        catch (Exception)
        {
            throw new FileValidationException("Invalid file format");
        }
    }

    static bool IsUtf8Encoded(byte[] fileContent)
    {
        return Encoding.UTF8.GetString(fileContent).Equals(Encoding.Default.GetString(fileContent));
    }
}