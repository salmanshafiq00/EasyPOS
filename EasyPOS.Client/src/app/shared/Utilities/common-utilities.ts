export class CommonUtils {
  
  // A static method for converting enum to an array of { id, name } pairs
  static enumToArray(enumObj: any): { id: number, name: string }[] {
    return Object.keys(enumObj)
      .filter(key => isNaN(Number(key)))
      .map(key => ({ id: enumObj[key], name: key }));
  }

  
  static ddmmyyyyToyyyymmdd(date: string): string{
    const dateParts = date.split('/');
    return `${dateParts[2]}-${dateParts[1]}-${dateParts[0]}`;
  }

}
