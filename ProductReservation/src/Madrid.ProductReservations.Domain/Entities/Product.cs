namespace Madrid.ProductReservations.Domain.Entities;
public class Product : EntityBase
{
	public string Name { get; private set; }

	protected Product()
	{
	}

	public Product(Guid id, string name)
		: base(id)
	{
		Id = id;
		Name = name;
	}

	public void ChangeName(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			throw new ArgumentException("Нельзя пустое имя", nameof(value));

		Name = value;
	}
}
