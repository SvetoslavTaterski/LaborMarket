import { Component } from '@angular/core';
import { HeaderComponent } from "../../header/header.component";
import { CardComponent } from "../../card/card.component";
import { FooterComponent } from "../../footer/footer.component";

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [HeaderComponent, CardComponent, FooterComponent],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent {

}
