import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FreeGamesComponent } from './free-games/free-games.component';
import { MyGamesComponent } from './my-games/my-games.component';

const routes: Routes = [
  { path: 'free', component: FreeGamesComponent },
  { path: 'my', component: MyGamesComponent },
  { path: '', redirectTo: '/free', pathMatch: 'full' },
  { path: '**', redirectTo: '/free' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { enableTracing: false })],
  exports: [RouterModule]
})
export class AppRoutingModule { }