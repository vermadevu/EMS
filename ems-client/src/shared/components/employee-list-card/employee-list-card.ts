import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { EmployeeListItem } from '../../models/employee-list-item';

@Component({
  selector: 'app-employee-list-card',
  imports: [
    RouterLink
  ],
  templateUrl: './employee-list-card.html',
  styleUrl: './employee-list-card.css',
})
export class EmployeeListCard {
    readonly title = input.required<string>();
    readonly employees = input.required<EmployeeListItem[]>();
    readonly viewAllRoute = input<string>();
}
