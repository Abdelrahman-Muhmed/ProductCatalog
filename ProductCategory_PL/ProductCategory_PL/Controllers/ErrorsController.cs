using Microsoft.AspNetCore.Mvc;
using ProductCatalog_BLL.Helpers.Error;
using ProductCatalog_DAL.Prsistence.Data;

namespace ProductCatalog_PL.Controllers
{
    public class ErrorsController : Controller
    {
        private readonly ProductContext _dbContext;
        public ErrorsController(ProductContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpGet("NotFound")] // For Not Found Product >>>>
        public ActionResult GetNotFound()
        {
            var products = _dbContext.Product.Find(50); // Now Am Try To Return This Item From Products But It's Not Found 

            if (products == null)
                return NotFound(new ApiResponse(404)); //But I will Using This more One Time So I will Create Class 
            return Ok(products);
        }

        [HttpGet("BadRequest")] //For Bad Request 
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("UnAuthrized")] //For unAuthrized 
        public ActionResult GetUnAuthrized()
        {
            return BadRequest(new ApiResponse(401));
        }

    }
}
