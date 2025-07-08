using Interfaces;

namespace Modules
{
  public class Field: IField
  {
    public string Name { get; set; }
    public string Type { get; set; }
    public bool Nullable { get; set; }
    public bool PrimaryKey { get; set; }
    public bool ForeignKey { get; set; }

    public Field(string name, string type, bool nullable, bool primaryKey, bool foreignKey)
    {
      this.Name = name;
      this.Type = type;
      this.Nullable = nullable;
      this.PrimaryKey = primaryKey;
      this.ForeignKey = foreignKey;
    }

    public void Properties()
    {
      Console.WriteLine($"NAME:\t\t\t{Name}");
      Console.WriteLine($"TYPE:\t\t\t{Type}");
      Console.WriteLine($"NULLABLE:\t\t{Nullable}");
      Console.WriteLine($"PRIMARY KEY:\t\t{PrimaryKey}");
      Console.WriteLine($"FOREIGN KEY:\t\t{ForeignKey}");
    }
  }

  class Model: IModel
  {
    private List<IField> Fields;

    public void DefineModel(List<IField> fields)
    {
      if (fields.Count == 0)
      {
        Console.WriteLine("Cannot accept empty Field List.");
        return;
      }

      this.Fields = fields;
    }
  }
}