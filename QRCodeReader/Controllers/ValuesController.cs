using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace QRCodeReader.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        [HttpGet()]
        [Route("api/Values/{*filePath}")]
        public async Task<string> ConvertToData(string filePath)
        {
            try
            {
                var fileStream = new FileStream(filePath, FileMode.Open);
                using (var client = new HttpClient())
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StreamContent(fileStream), "file", "LHfile");
                    var response = await client.PostAsync("http://api.qrserver.com/v1/read-qr-code/", formData);
                    var qrCodeData = await response.Content.ReadAsStringAsync();
                    return qrCodeData.ToString();
                }
            }
            catch (FileNotFoundException ex)
            {
                return "File not found, please check file path";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
