export class CommonUtils {
  
  // A static method for converting enum to an array of { id, name } pairs
  static enumToArray(enumObj: any): { id: number, name: string }[] {
    return Object.keys(enumObj)
      .filter(key => isNaN(Number(key)))
      .map(key => ({ id: enumObj[key], name: key }));
  }

}
