using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MiniCrmApi.Models;
using MiniCrmApi.Services;

namespace MiniCrmApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            List<Order> orderList = await orderService.GetAllAsync();
            return Ok(orderList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            Order order = await orderService.GetByIdAsync(id);
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] Order order)
        {
            await orderService.AddAsync(order);
            /*
             * Bu satır, şu mesajı istemciye gönderiyor gibi düşünebilirsin:
               “Order başarıyla oluşturuldu ✅
               Yeni order’a erişmek istersen → /api/Order/5 adresine GET isteği at.”
               Sonuçta dönen http cevabı
               HTTP/1.1 201 Created
               Location: https://localhost:5001/api/Order/5
               Content-Type: application/json; charset=utf-8
               
               Kısa açıklama: 201 Created döndürür.
               nameof(GetOrderById) ile Location: urlnin oluşmasını sağlar.
               new { id = order.Id } ile anonim sınıf oluşturuyoruz, Id propertysine orderın idsini
               atıyoruz, ardından bu id kullanılarak GetOrderById(int id) parametresine argüman olarak gidiyor,
               https://localhost:5001/api/Order/5 o anki oluşan id ne ise o yazacak son kısımda örnek olarak bu yapı
               oluşuyor, son kısımda order nesnesi json formatında dönüş yapıyor, entityde ne varsa.

               201 Created döndürür.
               nameof(GetOrderById) ile Location header’ında URL’nin oluşmasını sağlar.
               new { id = order.Id } ile anonim bir nesne oluşturuyoruz; Id property’sine order’ın ID’si atanır.
               Bu değer, GetOrderById(int id) metodunun id parametresine argüman olarak gider.
               Örneğin, URL https://localhost:5001/api/Order/5 şeklinde oluşur.
               Son parametre olan order ise response body’de JSON olarak döner (entitydeki tüm alanlarla birlikte).
             * 
             */
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            await orderService.UpdateAsync(id, order);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await orderService.DeleteAsync(id);
            return NoContent();
        }
    }
}
