
import { Directive, ElementRef, HostListener, Input, Output, EventEmitter } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: 'input[numbersOnly]'
})
export class NumberDirective {

@Output() valueChange = new EventEmitter()
  constructor(private _el: ElementRef) { }

  @HostListener('input', ['$event']) onInputChange(event: any) {
    const initalValue = this._el.nativeElement.value;
     const newValue1 = initalValue.replace(/[^0-9]*/g, '');
    const newValue = initalValue.replace(/[^0-9,+,.]*/g, '');
       this._el.nativeElement.value = newValue;
       this._el.nativeElement.value = newValue1;
       this.valueChange.emit(newValue1);
       this.valueChange.emit(newValue);
    if ( initalValue !== this._el.nativeElement.value) {
      event.stopPropagation();
    }
  }

}