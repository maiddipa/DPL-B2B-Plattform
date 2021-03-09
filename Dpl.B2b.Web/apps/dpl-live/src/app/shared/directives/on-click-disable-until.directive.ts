import {
  Directive,
  ElementRef,
  HostListener,
  Input,
  OnDestroy,
  Renderer2,
} from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, first, tap } from 'rxjs/operators';

@Directive({
  selector: '[dplOnClickDisableUntil]',
})
export class OnClickDisableUntilDirective implements OnDestroy {
  @Input('dplOnClickDisableUntil') obs: Observable<any>;

  constructor(private renderer: Renderer2, private el: ElementRef) {}

  @HostListener('click')
  onClick() {
    this.renderer.setAttribute(this.el.nativeElement, 'disabled', 'true');

    this.obs
      .pipe(
        first(),
        catchError((obs) => of(null)),
        tap(() => {
          this.renderer.removeAttribute(this.el.nativeElement, 'disabled');
        })
      )
      .subscribe();
  }

  ngOnDestroy(): void {}
}
