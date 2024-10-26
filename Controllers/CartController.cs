using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace WebApp.Models;

[Authorize]
public class CartController : Controller
{
    OrganicContext context;
    public CartController(OrganicContext context){
        this.context = context;
    }
    [HttpPost]
    public IActionResult Add(Cart obj){
        string? memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(string.IsNullOrEmpty(memberId)){
            return Redirect("/auth/login");
        }
     
        obj.MemberId = memberId;
        // obj.CreatedDate = DateTime.Now;
        // obj.UpdatedDate = DateTime.Now;
        // context.Carts.Add(obj);
        // context.SaveChanges();
      
        if(context.Carts.Any(p => p.MemberId == obj.MemberId && p.ProductId == obj.ProductId)){
            Cart? cart = context.Carts.FirstOrDefault(p => p.MemberId == obj.MemberId && p.ProductId == obj.ProductId);
            if(cart != null){
                cart.Quantity += obj.Quantity;
                cart.UpdatedDate = DateTime.Now;
                context.Carts.Update(cart);
            }
        }else{
            obj.CreatedDate = DateTime.Now;
            obj.UpdatedDate = DateTime.Now;
            context.Carts.Add(obj);
        }
        context.SaveChanges();
        return Redirect("/cart");
    }
    public IActionResult Index(){
        //Miss
        ViewBag.Departments = context.Departments.ToList();
       
        ViewBag.Categories = context.Categories.ToList();

        string? memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(string.IsNullOrEmpty(memberId)){
            return Redirect("/auth/login");
        }
        return View(context.Carts.Include(p => p.Product).Where(p => p.MemberId == memberId).ToList());
    }
    //Tự làm
    public IActionResult Delete(int id){
         string? memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(memberId))
        {
            return Redirect("/auth/login");
        }
        // Tìm sản phẩm trong giỏ hàng của người dùng hiện tại
        Cart? cartItem = context.Carts.FirstOrDefault(c => c.CartId == id && c.MemberId == memberId);
        if (cartItem == null)
         {
            return NotFound();
        }
        context.Carts.Remove(cartItem);
        context.SaveChanges();
        return Redirect("/cart");
    }
    //Tự làm
    [HttpPost]
    public IActionResult Edit(int id, short quantity){
       // Kiểm tra nếu số lượng nhỏ hơn 1
    if (quantity < 1)
    {
        ModelState.AddModelError("", "Quantity must be at least 1.");
        return Redirect("/cart");
    }

    string? memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(memberId))
    {
        return Redirect("/auth/login");
    }

    // Tìm sản phẩm trong giỏ hàng của người dùng hiện tại
    Cart? cartItem = context.Carts.FirstOrDefault(c => c.CartId == id && c.MemberId == memberId);
    if (cartItem == null)
    {
        return NotFound();
    }

    // Cập nhật số lượng sản phẩm và ngày cập nhật
    cartItem.Quantity = quantity;
    cartItem.UpdatedDate = DateTime.Now;

    // Lưu thay đổi vào cơ sở dữ liệu
    context.Carts.Update(cartItem);
    context.SaveChanges();

    // Chuyển hướng lại trang giỏ hàng
    return RedirectToAction("Index");
    }
}