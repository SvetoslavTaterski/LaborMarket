import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { provideRouter, Routes } from '@angular/router';
import { HomePageComponent } from './app/components/pages/home-page/home-page.component';
import { WorkersPageComponent } from './app/components/pages/workers-page/workers-page.component';
import { PositionsPageComponent } from './app/components/pages/positions-page/positions-page.component';
import { EmployersPageComponent } from './app/components/pages/employers-page/employers-page.component';

const routes: Routes = [
  { path: '', component: HomePageComponent }, // Default route
  { path: 'workers', component: WorkersPageComponent },
  { path: 'positions', component: PositionsPageComponent},
  { path: 'employers', component: EmployersPageComponent},
];

bootstrapApplication(AppComponent, {
  providers: [provideRouter(routes)],
}).catch((err) => console.error(err));
