import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-display-paper',
  templateUrl: './display-paper.component.html',
  styleUrls: ['./display-paper.component.css']
})
export class DisplayPaperComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
  }

}
