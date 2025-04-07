using System.Net.Sockets;
using AutoMapper;
using AutoMapperPratice;
using AutoMapperPratice.Mapping.MappingProfile.AutoMapper;

Console.WriteLine("Hello, World!");

var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
var mapper = config.CreateMapper();

var user = new Product()
{
    Price = 500,
    Name = "Rider",
    Description = "Desc",
    Id = 1
};
var user2 = new Product()
{
    Price = 1500,
    Name = "VCS",
    Description = "22Desc",
    Id = 2
};
var dto = new ProductListDto()
{
    Price = 500,
    Name = "Rider",
    Description = "Desc",
    Id = 1
};

var dto2 = new ProductListDto()
{
    Price = 1500,
    Name = "VCS",
    Description = "22Desc",
    Id = 2
};

var DTOlist = new List<ProductListDto>();
    DTOlist.Add(dto);
    DTOlist.Add(dto2);

var EntityList = new List<Product>();
    EntityList.Add(user);
    EntityList.Add(user2);



// -------------------


var entity = mapper.Map<ProductListDto, Product>(dto, user);

var entity2 = mapper.Map<Product, ProductListDto>(user2, dto);

var entity3 = mapper.Map(user2, dto);

var listx = mapper.Map<List<ProductListDto>>(EntityList);

Console.WriteLine("listx Type : "+listx.GetType());
foreach (var item in listx)
{
    Console.WriteLine(item.ImageUrl);
}