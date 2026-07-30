import { Component, input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-empty-state',
  imports: [
    MatIconModule,
    RouterLink
  ],
  templateUrl: './empty-state.html',
  styleUrl: './empty-state.css',
})
export class EmptyStateComponent {
  readonly icon = input('inbox');
  readonly title = input.required<string>();
  readonly subtitle = input<string>();
  readonly buttonText = input<string>();
  readonly buttonIcon = input('add');
  readonly buttonRoute = input<string>();
}
