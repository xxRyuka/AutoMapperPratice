# AutoMapper Kullanımı

AutoMapper, .NET uygulamalarında nesneler arası dönüşümü otomatikleştiren güçlü bir kütüphanedir. Özellikle **DTO (Data Transfer Object)** ve **Entity** nesneleri arasındaki dönüşümleri kolaylaştırır. Bu sayede veri taşıma işlemleri daha verimli hale gelir.

## Neden AutoMapper Kullanmalıyız?

AutoMapper, aşağıdaki gibi birçok fayda sağlar:

- **Kolay ve Temiz Kod:** Her seferinde manuel olarak dönüşüm yazmak yerine, AutoMapper sayesinde dönüşümleri tek bir satırla halledebilirsiniz.
- **Kod Tekrarını Önler:** Aynı dönüşüm işlemlerini her yerde tekrar etmek yerine merkezi bir yerde tanımlayarak kod tekrarını önlersiniz.
- **Daha Az Hata:** Dönüşüm işlemleri manuel olarak yazıldığında hata yapma olasılığı artar. AutoMapper, bu işlemi otomatik hale getirerek hata yapma olasılığını azaltır.
- **Gelişmiş Özelleştirme:** AutoMapper, basit eşleştirmeler dışında, dönüşüm işlemlerinizi özelleştirebilir. Örneğin, bir property'yi belirli bir formatta dönüştürmek için `ConvertUsing()` metodunu kullanabilirsiniz.

---

## Başlangıç Setup'ı (Tek Seferlik)

Öncelikle AutoMapper kullanabilmek için aşağıdaki adımları takip edin.

### Modeller

```csharp
// Models/Product.cs
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
}

// Models/ProductDto.cs
public class ProductDto
{
    public int Id { get; set; }
    public string ProductName { get; set; }
}
```

### MappingProfile.cs

```csharp
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
            .ReverseMap(); // Dto'dan Entity'ye dönüşüm de yapılsın
    }
}
```

### Program.cs

```csharp
var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
var mapper = config.CreateMapper();
```

---

## AutoMapper Kullanım Senaryoları

### 1. `mapper.Map<TDestination>(source)`

```csharp
var product = new Product { Id = 1, Name = "Kalem" };
var dto = mapper.Map<ProductDto>(product);

Console.WriteLine($"DTO: {dto.Id}, {dto.ProductName}");
```

**Açıklama:**
- Sadece hedef tipi (`ProductDto`) verilir ve kaynak tipi (`Product`) otomatik anlaşılır.

### 2. `mapper.Map<TSource, TDestination>(source)`

```csharp
var product2 = new Product { Id = 2, Name = "Defter" };
var dto2 = mapper.Map<Product, ProductDto>(product2);

Console.WriteLine($"DTO2: {dto2.Id}, {dto2.ProductName}");
```

**Açıklama:**
- Hem kaynak hem hedef tipi açıkça belirtilir. Bu özellikle tip karmaşası olduğunda veya refactor işlemlerinde kullanışlıdır.

### 3. `mapper.Map(source, destination)`

```csharp
var existingDto = new ProductDto();
mapper.Map(product2, existingDto);

Console.WriteLine($"Var olan DTO: {existingDto.Id}, {existingDto.ProductName}");
```

**Açıklama:**
- Bu kullanımda yeni bir nesne oluşturulmaz, mevcut nesne (`existingDto`) üzerinde dönüşüm yapılır.

### 4. Liste Dönüşümü: `mapper.Map<List<TDest>>(List<TSource>)`

```csharp
var products = new List<Product>
{
    new Product { Id = 1, Name = "Silgi" },
    new Product { Id = 2, Name = "Cetvel" }
};

var dtoList = mapper.Map<List<ProductDto>>(products);

foreach (var item in dtoList)
    Console.WriteLine($"List DTO: {item.Id}, {item.ProductName}");
```

**Açıklama:**
- Bir listeyi dönüştürmek için aynı `Map` fonksiyonu yeterlidir. `List<Product>` → `List<ProductDto>` dönüşümünü sağlar.

### 5. Dizi Dönüşümü: `mapper.Map<TDest[]>(TSource[])`

```csharp
var productArray = new Product[]
{
    new Product { Id = 1, Name = "Kitap" },
    new Product { Id = 2, Name = "Kalemlik" }
};

var dtoArray = mapper.Map<ProductDto[]>(productArray);

foreach (var dto in dtoArray)
    Console.WriteLine($"Array DTO: {dto.Id}, {dto.ProductName}");
```

**Açıklama:**
- Bu kullanım, dizileri dönüştürmek için kullanılır. Liste dönüşümü ile aynıdır.

### 6. Runtime Bilinmeyen Tiplerle: `Map(object, object)`

```csharp
object dynamicSource = new Product { Id = 5, Name = "Flüt" };
object dynamicTarget = new ProductDto();

var result = mapper.Map(dynamicSource, dynamicTarget);

Console.WriteLine($"Dynamic Map: {((ProductDto)result).ProductName}");
```

**Açıklama:**
- Bu kullanımda tipler çalışma zamanında belirlenir. `object` üzerinden dönüşüm yapılabilir, ancak performans açısından risklidir.

### 7. `ConvertUsing()` ile Özel Dönüşüm

```csharp
CreateMap<Product, ProductDto>()
    .ConvertUsing(src => new ProductDto
    {
        Id = src.Id,
        ProductName = src.Name.ToUpper()
    });
```

**Açıklama:**
- Burada, `Product` nesnesini `ProductDto`'ya dönüştürürken, dönüşümü özelleştiriyoruz. Örneğin, `Name` property'sini büyük harfe dönüştürüyoruz.

---

## Özet Tablo: Map Kullanım Türleri

| Kullanım Şekli | Açıklama | Nerede Kullanılır |
| --- | --- | --- |
| `Map<TDest>(source)` | En sade kullanım | Genel |
| `Map<TSrc, TDest>(source)` | Tip güvenliği yüksek | Refactor durumları |
| `Map(source, target)` | Var olan nesneye yaz | Update işlemleri |
| `Map<List<TDest>>(List<TSrc>)` | Liste dönüşümü | DTO listeleri |
| `Map<TDest[]>(TSource[])` | Dizi dönüşümü | Performanslı dizi dönüşümleri |
| `Map(object, object)` | Dinamik map | Generic servisler |
| `ConvertUsing()` | Özelleştirilmiş dönüşüm | Formatlama, hesaplama vb. |

---



