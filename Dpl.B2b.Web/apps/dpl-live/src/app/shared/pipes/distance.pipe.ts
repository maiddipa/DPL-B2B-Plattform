import { DecimalPipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';

export type DistancePipeFormat = 'km';

@Pipe({
  name: 'distance',
})
export class DistancePipe implements PipeTransform {
  constructor(private numberPipe: DecimalPipe) {}
  transform(valueInMeters: number, format: DistancePipeFormat = 'km'): string {
    const convertedValue = Math.round(valueInMeters / 1000);
    const formattedValue = this.numberPipe.transform(convertedValue);
    return `${formattedValue} km`;
  }
}
