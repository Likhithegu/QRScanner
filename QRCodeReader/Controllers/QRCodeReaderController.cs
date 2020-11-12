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
    public class QRCodeReaderController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet()]
        [Route("api/QRCodeReader/{*filePath}")]
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

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}