import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { SignInComponent } from './auth/sign-in/sign-in.component';
import { UserBageComponent } from './user-bage/user-bage.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'user', component: UserBageComponent },
  { path: 'sign-in', component: SignInComponent, outlet: 'popup' },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '**', redirectTo: '/home' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { enableTracing: false })],
  exports: [RouterModule]
})
export class AppRoutingModule { }