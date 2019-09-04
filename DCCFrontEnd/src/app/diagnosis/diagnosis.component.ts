import { Component, OnInit, OnChanges, SimpleChanges, AfterContentInit, AfterViewInit } from '@angular/core';
import { DiagnosisService } from './service/diagnosis.service';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { faCapsules } from '@fortawesome/free-solid-svg-icons';
import { Route, Router } from '@angular/router';
import { MatDialog } from '@angular/material';
import { DrugDetailsComponent } from './drug-details/drug-details.component';

@Component({
  selector: 'app-diagnosis',
  templateUrl: './diagnosis.component.html',
  styleUrls: ['./diagnosis.component.css']
})
export class DiagnosisComponent implements OnInit {
  loading = true;

   capsola = faCapsules ;
  isLinear = false;
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  form: FormGroup;
  isEditable = true;


  bodyAreas: any;
  bodyarea;
  syptoms;
  drugs;
  drug;
  chang1;
  chang2;
  chang3;
  headShow = false;
  ribcageShow = false;
  eyeShow = false;
  mainImg = true;
  earShow = false;
  armShow = false;
  stomachShow = false;
  footShow = false;
  model: any = {};
  symtopm: any = {} ;
  headache = false;
  constructor(private dignosisService: DiagnosisService, private fb: FormBuilder,
    private _dialog: MatDialog,
    private router:Router) { }
  myStyle: object = {};
  myParams: object = {};
  width: number = 100;
  height: number = 100;
  ngOnInit() {
    this.loading = false;
    this.myStyle = {
      'position': 'fixed',
      'width': '100%',
      'height': '100%',
      'z-index': 1,
      'top': 0,
      'left': 0,
      'right': 0,
      'bottom': 0,
      'background': ' rgb(0, 55, 99, 0.5)'
    };

    this.myParams = {
      particles: {
        number: {
          value: 100,
        },
        color: {
          value: '#003763'
        },
        shape: {
          type: 'circle',
        },
      }
    };
    console.log(this.model)

    this.dignosisService.getAllBodyAreas().subscribe((r) => {
      this.bodyAreas = r;
      console.log(r);
    })
    this.firstFormGroup = this.fb.group({
      firstCtrl: ['', Validators.required]
    });
    this.secondFormGroup = this.fb.group({
      secondCtrl: ['', Validators.required]
    });

  }
  createForm() {
    this.form = this.fb.group({
      requestId: [0, Validators.required],
      symptomId: [0, Validators.required],
      userId: [0, Validators.required],
      drugId: [0, Validators.required],
      bodyAreasId: [0, Validators.required],
      timeCreated: []

    })
  }
  changeValue(event) {
    console.log(this.model, "<<<<<<<<", event);

    if (event == 1) {
      this.headShow = true;
      this.ribcageShow = false;
      this.mainImg = false;
      this.eyeShow = false;
      this.earShow = false;
      this.armShow = false;
      this.stomachShow = false;
      this.footShow = false;
      this.headache = false;
    }
    else if (event == 2) {
      this.headShow = false;
      this.ribcageShow = false;
      this.mainImg = false;
      this.eyeShow = false;
      this.armShow = false;
      this.earShow = false;
      this.stomachShow = true;
      this.footShow = false;
      this.headache = false;
    }
    else if (event == 3 || event == 4) {
      this.earShow = true;
      this.headShow = false;
      this.ribcageShow = false;
      this.mainImg = false;
      this.eyeShow = false;
      this.armShow = false;
      this.stomachShow = false;
      this.footShow = false;
      this.headache = false;
    }
    else if (event == 5) {
      this.headShow = false;
      this.ribcageShow = false;
      this.mainImg = false;
      this.eyeShow = true;
      this.earShow = false;
      this.armShow = false;
      this.stomachShow = false;
      this.footShow = false;
      this.headache = false;
    }
    else if (event == 7) {
      this.headShow = false;
      this.ribcageShow = false;
      this.mainImg = false;
      this.eyeShow = false;
      this.earShow = false;
      this.armShow = true;
      this.stomachShow = false;
      this.footShow = false;
      this.headache = false;
    } else {
      this.headShow = false;
      this.ribcageShow = false;
      this.mainImg = true;
      this.eyeShow = false;
      this.earShow = false;
      this.armShow = false;
      this.stomachShow = false;
      this.footShow = false;
      this.headache = false;

    }
    this.dignosisService.getAllSymptom(event).subscribe((r) => {
      this.syptoms = r;
      console.log(r);

    });

  }
  head() {
    this.headShow = true;
    this.ribcageShow = false;
    this.mainImg = false;
    this.model.bodyAreasId = 1;
    this.eyeShow = false;
    this.earShow = false;
    this.armShow = false;
    this.stomachShow = false;
    this.footShow = false;
    this.headache = false;
    this.getSypmtom(this.model.bodyAreasId);


  }

  stomach() {
    this.ribcageShow = false;
    this.headShow = false;
    this.model.bodyAreasId = 2;
    this.mainImg = false;
    this.eyeShow = false;
    this.earShow = false;
    this.armShow = false;
    this.stomachShow = true;
    this.footShow = false;
    this.headache = false;
    this.getSypmtom(this.model.bodyAreasId);

  }
  eye() {
    this.headShow = false;
    this.ribcageShow = false;
    this.mainImg = false;
    this.eyeShow = true;
    this.model.bodyAreasId = 5;
    this.earShow = false;
    this.armShow = false;
    this.stomachShow = false;
    this.footShow = false;
    this.headache = false;
    this.getSypmtom(this.model.bodyAreasId);

  }
  ear() {
    this.headShow = false;
    this.ribcageShow = false;
    this.mainImg = false;
    this.eyeShow = false;
    this.model.bodyAreasId = 3;
    this.earShow = true;
    this.armShow = false;
    this.stomachShow = false;
    this.footShow = false;
    this.headache = false;
    this.getSypmtom(this.model.bodyAreasId);

  }

  arm() {
    this.headShow = false;
    this.ribcageShow = false;
    this.mainImg = false;
    this.eyeShow = false;
    this.model.bodyAreasId = 7;
    this.earShow = false;
    this.armShow = true;
    this.footShow = false;
    this.headache = false;
    this.stomachShow = false;
    this.getSypmtom(this.model.bodyAreasId);

  }
  ribcage() {

  }
  foot() {

  }
  changeValue2(event) {
    console.log(event,this.symtopm.symtopmId, "22")
    this.model.symptomId = event.symptomId;
    console.log(this.model.symptomId)
    if (event.symptomId == 1 && event.symptomName == "headache") {
      this.headache = true;
 
    }
    else if (event == 2) {
      this.headache = false;
  
    }
    this.dignosisService.getDrugBySymptomId(event.symptomId).subscribe((r) => {
      console.log(r);
      this.drugs = r;
    })
 
  }
  showDrug = false ;
  changeValue3(event) {
    console.log(event)
    this.dignosisService.getDrugById(event).subscribe((r) => {
      this.drug = r;
      this.showDrug = true ;
    })
  }
  next() {
    console.log(this.model);
    let currentUser =JSON.parse(localStorage.getItem('currentUser')) ;
    console.log()
    this.model.userId = currentUser.id ;
    this.model.requestId = 0 ;
    console.log(this.model);

    this.dignosisService.createRequest(this.model).subscribe((r)=>{
      console.log(r);
      this.router.navigate(['/my-history']);
    })
  }
  getSypmtom(event) {
    this.dignosisService.getAllSymptom(event).subscribe((r) => {
      this.syptoms = r;
      console.log(r);

    });

  }
  goToDrugDetails(id){
    this.router.navigate([`/diagnosis/drug-details/${id}`])
  }
  drugDetails(id){
    const dialogRef = this._dialog.open(DrugDetailsComponent, {
      width: '460px',
      height: '250px',
      data: id
    },
     );

  }
}
