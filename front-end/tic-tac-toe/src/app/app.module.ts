import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AddGameComponent } from './free-game/add-game/add-game.component';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FreeGameComponent } from './free-game/free-game.component';
import { PlayerLineComponent } from './free-game/player-line/player-line.component';
import { FreeGamesComponent } from './free-games/free-games.component';
import { BASE_API_URL } from './generated/clients';
import { LoaderComponent } from './loader/loader.component';
import { MyGamesComponent } from './my-games/my-games.component';
import { ItemRectangleComponent } from './free-game/item-rectangle/item-rectangle.component';

@NgModule({
  declarations: [
    AppComponent, FreeGamesComponent, MyGamesComponent, FreeGameComponent, PlayerLineComponent, LoaderComponent, AddGameComponent, ItemRectangleComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [
    { provide: BASE_API_URL, useValue: 'https://localhost:5001' },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }