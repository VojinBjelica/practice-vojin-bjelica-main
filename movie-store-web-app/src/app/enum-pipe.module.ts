import { NgModule } from '@angular/core';
import { EnumToStringPipe } from './enum-to-string.pipe';

@NgModule({
  declarations: [EnumToStringPipe],
  exports: [EnumToStringPipe]
})
export class EnumPipeModule { }