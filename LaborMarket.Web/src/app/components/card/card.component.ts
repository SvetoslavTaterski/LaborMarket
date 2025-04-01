import { Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-card',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './card.component.html',
  styleUrl: './card.component.scss',
})
export class CardComponent {
  @Input() buttonLabel: string = '';
  @Input() cardInfo: string = '';
  @Input() iconClass: string = '';
  @Input() buttonLink: string = '';
}
