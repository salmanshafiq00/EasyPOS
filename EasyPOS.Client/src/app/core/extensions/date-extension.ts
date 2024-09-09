// date-extensions.ts
interface Date {
  toYYYYMMDD(): string;
}

// date-extensions.ts
Date.prototype.toYYYYMMDD = function (): string {
  const year = this.getFullYear();
  const month = ('0' + (this.getMonth() + 1)).slice(-2);
  const day = ('0' + this.getDate()).slice(-2);
  return `${year}-${month}-${day}`;
};
