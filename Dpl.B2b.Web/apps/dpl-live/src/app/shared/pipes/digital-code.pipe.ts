import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'digitalCode',
})
export class DigitalCodePipe implements PipeTransform {
  transform(value: string, ...args: any[]): any {
    if (!value) {
      return undefined;
    }

    return value.toUpperCase();
  }
}
