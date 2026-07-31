import { Component, input } from '@angular/core';

@Component({
  selector: 'app-detail-item',
  imports: [],
  templateUrl: './detail-item.html',
  styleUrl: './detail-item.css',
})
export class DetailItemComponent {
  readonly label = input.required<string>();
  readonly value = input<string | number | null | undefined>('');
}
