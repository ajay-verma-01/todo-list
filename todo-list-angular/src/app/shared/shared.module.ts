import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ErrorsComponent } from './errors/errors.component';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [ErrorsComponent],
  imports: [CommonModule, HttpClientModule],
  exports: [ErrorsComponent, HttpClientModule],
})
export class SharedModule {}
