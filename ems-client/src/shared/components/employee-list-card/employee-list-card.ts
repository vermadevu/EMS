import { Component, inject, input } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { EmployeeListItem } from '../../models/employee-list-item';
import { StatusBadge } from '../status-badge/status-badge';

@Component({
  selector: 'app-employee-list-card',
  imports: [
    StatusBadge
  ],
  templateUrl: './employee-list-card.html',
  styleUrl: './employee-list-card.css',
})
export class EmployeeListCard {
    readonly title = input.required<string>();
    readonly employees = input.required<EmployeeListItem[]>();
    readonly route = input.required<string>();
    private readonly router = inject(Router);

    go(){
      this.router.navigateByUrl(this.route());
    }
    
}
