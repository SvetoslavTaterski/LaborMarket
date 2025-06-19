import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideRouter, Routes } from '@angular/router';
import { HomePageComponent } from './app/components/pages/home-page/home-page.component';
import { WorkersPageComponent } from './app/components/pages/workers-page/workers-page.component';
import { PositionsPageComponent } from './app/components/pages/positions-page/positions-page.component';
import { EmployersPageComponent } from './app/components/pages/employers-page/employers-page.component';
import { provideHttpClient } from '@angular/common/http';
import { LoginPageComponent } from './app/components/pages/login-page/login-page.component';
import { RegisterPageComponent } from './app/components/pages/register-page/register-page.component';
import { importProvidersFrom } from '@angular/core';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ProfilePageComponent } from './app/components/pages/profile-page/profile-page.component';
import { PositionPageComponent } from './app/components/pages/position-page/position-page.component';
import { EditPositionPageComponent } from './app/components/pages/edit-position-page/edit-position-page.component';

const routes: Routes = [
  { path: '', component: HomePageComponent }, // Default route
  { path: 'workers', component: WorkersPageComponent },
  { path: 'positions', component: PositionsPageComponent },
  { path: 'employers', component: EmployersPageComponent },
  { path: 'login', component: LoginPageComponent },
  { path: 'sign-in', component: RegisterPageComponent },
  { path: 'profile', component: ProfilePageComponent},
  { path: 'position/:id', component: PositionPageComponent },
  { path: 'edit-position/:id', component: EditPositionPageComponent },
  { path: '**', redirectTo: '' } // Redirect unmatched routes to the homepage
];

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes),
    provideHttpClient(),
    importProvidersFrom(BrowserAnimationsModule),
    importProvidersFrom(
      ToastrModule.forRoot({
        timeOut: 3000, // Duration of toast
        positionClass: 'toast-top-right', // Position of toast
        preventDuplicates: true,
      })
    ),
  ],
}).catch((err) => console.error(err));
