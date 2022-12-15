using Microsoft.AspNetCore.Mvc;
using Context;

namespace CSharp.Controllers;

[ApiController]
[Route("Products")]
public class ProductsController : ControllerBase{

    private readonly ApplicationDbContext _DBContext;

    public ProductsController(ApplicationDbContext dbContext)
    {
        this._DBContext = dbContext; 
    } 

    [HttpGet]
    [Route("GetAll")]
    public IActionResult GetAll()
    {
        var product = this._DBContext.Products.ToList();
     return Ok(product);   
    }

    [HttpGet]
    [Route("GetProductById/{id}")]
    public IActionResult GetById(int id)
    {
        var product = this._DBContext.Products.FirstOrDefault(p=> p.Id == id);
        return Ok(product);
    }

    // delete
    [HttpDelete("Remove/{id}")]
    public IActionResult Remove(int id)
    {
        var product = this._DBContext.Products.FirstOrDefault(p=> p.Id == id);
        if(product != null){
            this._DBContext.Remove(product);
            this._DBContext.SaveChanges();
            return Ok(true);
        }else{
            return Ok(false);
        }
        
    }

    // create
    [HttpPost("Create")]
    public IActionResult Create([FromBody] Product _product)
    {
        var product = this._DBContext.Products.FirstOrDefault(p=> p.Name == _product.Name);
        if(product == null){
            this._DBContext.Products.Add(_product);
            this._DBContext.SaveChanges();
        }else{
            product.Name= _product.Name;
            product.Price= _product.Price;
            this._DBContext.SaveChanges();
        }
        return Ok(product);
    }
}