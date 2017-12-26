using Conscience.Application.Services;
using Conscience.DataAccess.Repositories;
using Conscience.Web.Logger;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Conscience.Web.Controllers.Api
{
    public class PictureUploadController : ApiController
    {
        private static string ImagesUrl = "/Content/images/uploaded/";
        private static string ServerUploadFolder = HttpContext.Current.Server.MapPath("~" + ImagesUrl);

        private readonly IUsersIdentityService _usersService;
        private readonly AccountRepository _accountsRepo;
        
        public PictureUploadController(IUsersIdentityService usersService, AccountRepository accountsRepo)
        {
            _usersService = usersService;
            _accountsRepo = accountsRepo;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync(HttpRequestMessage request)
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;

            try
            {
                Log4NetLogger.Current.WriteDebug("Files: " + httpRequest.Files);
                Log4NetLogger.Current.WriteDebug("Count: " + httpRequest.Files.Count);
                if (httpRequest.Files.Count > 0)
                {
                    var file = httpRequest.Files[0];

                    Log4NetLogger.Current.WriteDebug("File: " + file);

                    var accountId = 0;

                    if (_usersService.CurrentUser != null)
                    {
                        Log4NetLogger.Current.WriteDebug("User: " + _usersService.CurrentUser);
                        accountId = _usersService.CurrentUser.Id;
                    }
                    else
                    {
                        accountId = int.Parse(httpRequest.QueryString["accountId"]);
                    }

                    Log4NetLogger.Current.WriteDebug("User id: " + accountId);

                    var fileName = accountId + "." + file.FileName.Split('.').Last().ToLowerInvariant();

                    Log4NetLogger.Current.WriteDebug("File name: " + fileName);

                    var filePath = ServerUploadFolder + fileName;

                    CompressImageAndSave(file.InputStream, filePath, fileName);

                    Log4NetLogger.Current.WriteDebug("File uploaded.");

                    var account = _accountsRepo.GetById(accountId);
                    account.PictureUrl = ImagesUrl + fileName + "?_ts=" + DateTime.Now.ToFileTime();
                    _accountsRepo.Modify(account);
                    account = _accountsRepo.GetById(account.Id);

                    result = Request.CreateResponse(HttpStatusCode.Created);
                    result.Content = new StringContent(account.PictureUrl, Encoding.UTF8, "text/plain");
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest);

                    var ms = new MemoryStream();
                    httpRequest.InputStream.CopyTo(ms);
                    var bytes = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Current.WriteError("Unable to upload the picture.", ex);
            }
            return result;
        }

        public static void CompressImageAndSave(Stream sourcePath, string targetPath, string fileName)
        {


            try
            {
                using (var image = Image.FromStream(sourcePath))
                {
                    float maxHeight = 900.0f;
                    float maxWidth = 900.0f;
                    int newWidth;
                    int newHeight;
                    Bitmap originalBMP = new Bitmap(sourcePath);
                    int originalWidth = originalBMP.Width;
                    int originalHeight = originalBMP.Height;

                    if (originalWidth > maxWidth || originalHeight > maxHeight)
                    {

                        // To preserve the aspect ratio  
                        float ratioX = (float)maxWidth / (float)originalWidth;
                        float ratioY = (float)maxHeight / (float)originalHeight;
                        float ratio = Math.Min(ratioX, ratioY);
                        newWidth = (int)(originalWidth * ratio);
                        newHeight = (int)(originalHeight * ratio);
                    }
                    else
                    {
                        newWidth = (int)originalWidth;
                        newHeight = (int)originalHeight;

                    }
                    var bitMAP1 = new Bitmap(originalBMP, newWidth, newHeight);
                    var imgGraph = Graphics.FromImage(bitMAP1);
                    var extension = fileName.Split('.').Last().ToLowerInvariant();
                    if (extension == "png" || extension == "gif")
                    {
                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);


                        bitMAP1.Save(targetPath, image.RawFormat);

                        bitMAP1.Dispose();
                        imgGraph.Dispose();
                        originalBMP.Dispose();
                    }
                    else if (extension == "jpg" || extension == "jpeg")
                    {

                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);
                        var jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                        var myEncoder = System.Drawing.Imaging.Encoder.Quality;
                        var myEncoderParameters = new EncoderParameters(1);
                        var myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        bitMAP1.Save(targetPath, jpgEncoder, myEncoderParameters);

                        bitMAP1.Dispose();
                        imgGraph.Dispose();
                        originalBMP.Dispose();

                    }


                }

            }
            catch (Exception)
            {
                throw;

            }
        }


        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}