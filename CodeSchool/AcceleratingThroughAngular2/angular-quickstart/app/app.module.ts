import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component'
import { CarPartsComponent } from './car-parts.component'
import { RacingDataService } from './racing-data.service';
import { HttpModule } from '@angular/http'

@NgModule({
  imports:      [ BrowserModule, HttpModule ],
  declarations: [ AppComponent, CarPartsComponent ],
  providers:    [ RacingDataService ],
  bootstrap:    [ AppComponent ]
})

export class AppModule { }
