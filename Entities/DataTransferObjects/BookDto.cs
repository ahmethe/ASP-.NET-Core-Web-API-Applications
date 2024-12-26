namespace Entities.DataTransferObjects
{

    /*
    <summary>
        [Serializable]
        public record BookDtoForUpdate(int Id, String Title, decimal Price);
        Açık bir ifade kullanırsak serializable attribute kullanmamıza gerek kalmaz.
    </summary>
    */

    public record BookDto
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public decimal Price { get; set; }
    }
}
