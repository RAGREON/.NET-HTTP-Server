using Interfaces;
using System.Collections.Generic;

namespace Interfaces
{
  // should contain a list of Fields
  public interface IRecord
  {
    Dictionary<string, IField> records { get; set; }
  }
}