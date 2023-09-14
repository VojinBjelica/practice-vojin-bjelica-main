import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'enumToString'
})
export class EnumToStringPipe implements PipeTransform {

  transform(value: any, enumType: any): string {
    if (enumType && value) {
      const enumKey = Object.keys(enumType).find(key => enumType[key] === value);
      return enumKey || '';
    }
    return '';
  }

}
