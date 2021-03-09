import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'highlight',
})
export class HighlightPipe implements PipeTransform {
  transform(value: string, searchTerm: string): any {
    if (!searchTerm) {
      return value;
    }

    const joinedSearchTerm = searchTerm
      .split(',')
      .map((i) => i.trim())
      .filter((i) => i.length > 0)
      .join('|');

    const re = new RegExp(joinedSearchTerm, 'gi'); //'gi' for case insensitive and can use 'g' if you want the search to be case sensitive.
    return (value || '').replace(re, '<mark>$&</mark>');
  }
}
