namespace AttendanceApi.Service
{
    public class AttendanceService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AttendanceService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task SendStudentIdAsync(int studentId)
        {
            var client = _httpClientFactory.CreateClient("Esp32Client");

            var json = System.Text.Json.JsonSerializer.Serialize(new { studentId });
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/studentId", content); // relative path, since BaseAddress is set

            Console.WriteLine($"Status: {response.StatusCode}");
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Body: {body}");
        }
    }
}
