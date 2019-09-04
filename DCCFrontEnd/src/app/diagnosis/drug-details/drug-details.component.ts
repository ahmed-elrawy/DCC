import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DiagnosisService } from '../service/diagnosis.service';

@Component({
  selector: 'app-drug-details',
  templateUrl: './drug-details.component.html',
  styleUrls: ['./drug-details.component.css']
})
export class DrugDetailsComponent implements OnInit {

  drug ;
  constructor(public dialogRef: MatDialogRef<DrugDetailsComponent>,
     @Inject(MAT_DIALOG_DATA) public data: any,private diagnosis: DiagnosisService) { 
      console.log(data)
      if (data !== 0 || null || undefined) {
        this.getDrugById(data);

      }else {
        
      }
     }

  ngOnInit() {
  }
getDrugById(id){
  this.diagnosis.getDrugById(id).subscribe((r)=> {
    this.drug = r ;
    console.log(this.drug);
  })
}
}
