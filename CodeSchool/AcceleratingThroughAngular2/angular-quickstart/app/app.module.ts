import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component'
import { CarPartsComponent } from './car-parts.component'
import { RacingDataService } from './racing-data.service';

@NgModule({
  imports:      [ BrowserModule ],
  declarations: [ AppComponent, CarPartsComponent ],
  providers:    [ RacingDataService ],
  bootstrap:    [ AppComponent ]
})

export class AppModule { }
