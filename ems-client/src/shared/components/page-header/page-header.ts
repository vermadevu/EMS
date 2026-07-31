import { Component, input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-page-header',
  imports: [
    RouterLink,
    MatIconModule
  ],
  templateUrl: './page-header.html',
  styleUrl: './page-header.css',
})
export class PageHeaderComponent {
  readonly title = input.required<string>();
  readonly subtitle = input<string>();
  readonly buttonText = input<string>();
  readonly buttonIcon = input('add');
  readonly buttonRoute = input<string | any[]>();
}
