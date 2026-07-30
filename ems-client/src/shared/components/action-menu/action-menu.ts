import { Component, input, output } from '@angular/core';
import { ActionMenuItem } from '../../models/action-menu-item';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-action-menu',
  imports: [MatIconModule],
  templateUrl: './action-menu.html',
  styleUrl: './action-menu.css',
})
export class ActionMenu {
  readonly actions = input.required<ActionMenuItem[]>();
  readonly actionSelected = output<string>();
}
