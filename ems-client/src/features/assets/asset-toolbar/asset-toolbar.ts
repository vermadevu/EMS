import { Component, input, output } from '@angular/core';

import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';

import { AssetStatus } from '../../../core/models/asset-status';
import { AssetType } from '../../../core/models/asset-type';

@Component({
  selector: 'app-asset-toolbar',
  imports: [
    FormsModule,
    MatIconModule
  ],
  templateUrl: './asset-toolbar.html',
  styleUrl: './asset-toolbar.css'
})
export class AssetToolbarComponent {

  readonly search = input('');

  readonly searchChange = output<string>();

  readonly assetTypeChange = output<number | undefined>();

  readonly statusChange = output<number | undefined>();

  readonly assetTypes = [
    { value: AssetType.Laptop, label: 'Laptop' },
    { value: AssetType.Desktop, label: 'Desktop' },
    { value: AssetType.Monitor, label: 'Monitor' },
    { value: AssetType.Keyboard, label: 'Keyboard' },
    { value: AssetType.Mouse, label: 'Mouse' },
    { value: AssetType.Headset, label: 'Headset' },
    { value: AssetType.Phone, label: 'Phone' },
    { value: AssetType.Tablet, label: 'Tablet' },
    { value: AssetType.Printer, label: 'Printer' },
    { value: AssetType.AccessCard, label: 'Access Card' },
    { value: AssetType.IdCard, label: 'ID Card' },
    { value: AssetType.Other, label: 'Other' }
  ];

  readonly statuses = [
    { value: AssetStatus.Available, label: 'Available' },
    { value: AssetStatus.Assigned, label: 'Assigned' }
  ];

  onSearch(value: string) {
    this.searchChange.emit(value);
  }

  onAssetTypeChange(value: string) {
    this.assetTypeChange.emit(
      value ? Number(value) : undefined
    );
  }

  onStatusChange(value: string) {
    this.statusChange.emit(
      value ? Number(value) : undefined
    );
  }

}