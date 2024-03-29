﻿namespace LESAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TruckWebService;

    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class PalletController : ControllerBase
    {
        private readonly ILogger<PalletController> logger;

        public PalletController(ILogger<PalletController> logger)
        {
            this.logger = logger;
        }

        [HttpGet("GeefLocatieInfoBijPalletnr/{palletNummer}")]
        public async Task<LocatieInfo> GeefLocatieInfoBijPalletnr(string palletNummer)
        {
            try
            {
                await using var serviceClient = new TruckWebServiceClient();
                var result = await serviceClient.GeefLocatieInfoBijPalletnrAsync(palletNummer);
                if (result != null) result.PalletsOpLocatie = null;//not needed for app.
                result ??= new LocatieInfo();
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return new LocatieInfo();
            }
        }

        [HttpGet("GeefPalletInfo/{palletNummer}")]
        public async Task<PalletInfo> GeefPalletInfo(string palletNummer)
        {
            try
            {
                await using var serviceClient = new TruckWebServiceClient();
                var result = await serviceClient.GeefPalletInfoAsync(palletNummer);
                if (result != null)
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return new PalletInfo();
        }


        [HttpPut("SluitVerzamelpallet/{palletNumber}/{productionNumber}")]
        public async Task<string> SluitVerzamelpallet(string palletNumber, int? productionNumber = 0)
        {
            var result = "";
            try
            {
                await using var serviceClient = new TruckWebServiceClient();
                result = await serviceClient.PickpalletDoelLocatieNaamVerzamelAanvoerOpdrachtAsync(palletNumber, productionNumber);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return result;
            }
        }
    }
}
