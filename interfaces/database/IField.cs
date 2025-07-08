namespace Interfaces 
{
  public interface IField
  {
    string Name { get; set; }
    string Type { get; set; }
    bool Nullable { get; set; }
    bool PrimaryKey { get; set; }
    bool ForeignKey { get; set; }

    void Properties();
  }
}