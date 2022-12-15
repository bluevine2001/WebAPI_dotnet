using Microsoft.AspNetCore.Mvc;
using Context;

namespace CSharp.Controllers;

[ApiController]
[Route("Cart")]
public class CartsController : ControllerBase{

    private readonly ApplicationDbContext _DBContext;

    public CartsController(ApplicationDbContext dbContext)
    {
        this._DBContext = dbContext; 
    } 

    [HttpGet]
    [Route("GetAll")]
    public IActionResult GetAll()
    {
        var cart = this._DBContext.Carts.ToList();
        return Ok(cart);   
    }

    [HttpGet]
    [Route("GetCartById/{id}")]
    public IActionResult GetCartById(int id)
    {
        var cart = this._DBContext.Carts.FirstOrDefault(p=> p.Id == id);
        return Ok(cart);
    }

    [HttpGet]
    [Route("GetCartOfUser/{id}")]
    public IActionResult GetCartOfUser(int id)
    {
        var cart = this._DBContext.Carts.FirstOrDefault(p=> p.userId == id);
        return Ok(cart);
    }

    // delete
    [HttpDelete("Remove/{id}")]
    public IActionResult Remove(int id)
    {
        var cart = this._DBContext.Carts.FirstOrDefault(p=> p.Id == id);
        if(cart != null){
            this._DBContext.Remove(cart);
            this._DBContext.SaveChanges();
            return Ok(true);
        }else{
            return Ok(false);
        }
        
    }

    // create
    [HttpPost("Create")]
    public IActionResult Create([FromBody] Cart _cart)
    {
        var cart = this._DBContext.Carts.FirstOrDefault(c=> c.Id == _cart.Id);
        if(cart == null){
            this._DBContext.Carts.Add(_cart);
            this._DBContext.SaveChanges();
        }else{
            cart.userId= _cart.userId;
            cart.TotalPrice= _cart.TotalPrice;
            this._DBContext.SaveChanges();
        }
        return Ok(cart);
    }

    // add product to cart
    [HttpPost("AddProductToCart/{cartId}")]
    public IActionResult AddProductToCart([FromBody] Product _product, int cartId)
    {   
        var cart = this._DBContext.Carts.FirstOrDefault(c=> c.Id == cartId);
        if(cart != null){
            cart.ProductIds += "|"+_product.Id;
            cart.TotalPrice += _product.Price;
            this._DBContext.SaveChanges();
        }
        return Ok();
    }

    // remove product from cart
    [HttpPost("RemoveProductFromCart/{cartId}")]
    public IActionResult RemoveProductFromCart(int cartId, [FromBody] Product _product)
    {
        var cart = this._DBContext.Carts.FirstOrDefault(c=> c.Id == cartId);
        if(cart != null){
            string[] updatedProductIds = cart.ProductIds.Split("|");
            string[] newpids = new string[updatedProductIds.Length];
            for (int i = 0; i < updatedProductIds.Length; i++)
            {
                if(Int32.Parse(updatedProductIds[i]) != _product.Id){
                    newpids[i] = updatedProductIds[i];
                }
            }
            string newIds = string.Join("|", newpids);
            cart.ProductIds = newIds;
            cart.TotalPrice -= _product.Price;
            this._DBContext.SaveChanges();
        }
        return Ok();
    }
}