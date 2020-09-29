<template>
    <v-container id="container" fluid>
        <v-form ref="form">
            <v-row id="row" justify="center">
                <v-col class="col" cols="12" md="8">
                    <h3 class="text-h3">Order Form</h3>
                    <v-row id="details-form">
                        <v-col class="col" cols="12" lg="6">
                            <h4 class="text-h4">Details</h4>
                            <v-divider class="mb-4"></v-divider>
                            <v-text-field
                                outlined rounded label="Full Name"
                                v-bind:rules="baseDetailsRules"
                                v-model="clientName"
                                maxlength="100"
                                required></v-text-field>
                            <v-text-field
                                outlined rounded label="Facebook Name"
                                v-bind:rules="baseDetailsRules"
                                maxlength="100"
                                v-model="fbName"
                                required></v-text-field>
                            <v-textarea
                                outlined rounded label="Complete Shipping Address"
                                v-bind:rules="baseDetailsRules"
                                v-model="shippingAddress"
                                maxlength="200"
                                required></v-textarea>
                            <v-text-field
                                outlined rounded label="Phone Number"
                                v-bind:rules="phoneNumberRules"
                                v-model="phoneNumber"
                                prefix="+63" maxlength="10">
                            </v-text-field>
                            <v-text-field
                                outlined rounded label="Brand Name"
                                v-bind:rules="baseDetailsRules"
                                v-model="brandName"
                                maxlength="100"
                                required></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col class="col" cols="12">
                            <h4 class="text-h4">Shades</h4>
                            <v-divider class="mb-4"></v-divider>
                            <div id="bundle-selection">
                                <h5 class="text-h5">Bundle Selection</h5>
                                <v-row>
                                    <v-col cols="12" sm="4">
                                        <v-subheader class="mb-4">Bundle List</v-subheader>
                                        <v-list>
                                            <v-list-item-group v-if="bundles && bundles.length">
                                                <v-list-item
                                                    v-for="bundle in bundles"
                                                    v-bind:key="bundle.Code"
                                                    v-on:click="selectBundle(bundle)">
                                                    <template>
                                                        <v-list-item-action>
                                                            <v-checkbox :input-value="selectedBundle.Code == bundle.Code"></v-checkbox>
                                                        </v-list-item-action>

                                                        <v-list-item-content>
                                                            <v-list-item-title> {{ bundle.Code }} </v-list-item-title>
                                                        </v-list-item-content>
                                                    </template>
                                                </v-list-item>
                                            </v-list-item-group>
                                            <h5 class="text-h5 text-center font-italic" v-else>No bundle available</h5>
                                        </v-list>
                                    </v-col>
                                    <v-col cols="12" sm="8" v-if="bundles && bundles.length">
                                        <v-subheader class="mb-4">Bundle Details</v-subheader>
                                        <div v-if="selectedBundle">
                                            <h3 class="text-h3" v-if="!!selectedBundle.Amount"> PHP {{ selectedBundle.Amount }} </h3>
                                            <span>{{ selectedBundle.Description }}</span>
                                        </div>
                                        <h5 class="text-h5" v-else>
                                            Select a bundle
                                        </h5>
                                    </v-col>
                                </v-row>
                            </div>
                            <div id="shade-selection" v-if="isNoSelectedBundle || bundleHasGelTint || bundleHasMatteTint">
                                <h5 class="text-h5">Shade Selection</h5>
                                <div id="gel-selection" v-show="bundleHasGelTint">
                                    <v-row justify="center" class="flex-column flex-column-reverse flex-md-row">
                                        <v-col cols="12" md="8">
                                            <h6 class="text-h6 text-center d-none d-md-block">Gel Lip & Cheek Tint</h6>
                                            <v-row>
                                                <v-col v-if="remainingGelTint != Infinity">
                                                    <span>{{ remainingGelTint }} remaining</span>
                                                </v-col>
                                                <v-col>
                                                    <h5 class="text-h5" v-if="isNoSelectedBundle"> PHP {{ gelTotalAmount }} </h5>
                                                    <span> {{ gelTotal }} pcs</span>
                                                </v-col>
                                            </v-row>
                                            <v-data-table
                                                v-bind:headers="shadeSelectorHeaders"
                                                v-bind:items="gelTints"
                                                v-bind:hide-default-footer="true"
                                                v-bind:disable-sort="true">
                                                <template v-slot:item.CustomShadeName="props">
                                                    <v-text-field
                                                        v-model="props.item.CustomShadeName"
                                                        v-on:focus="onColumnFocus($event, props.item, 'CustomShadeName')">
                                                    </v-text-field>
                                                    <span
                                                        class="caption red--text"
                                                        v-show="props.item.ValidationMessages.has('CustomShadeName')"> {{ props.item.ValidationMessages.get('CustomShadeName') }}
                                                    </span>
                                                </template>
                                                <template v-slot:item.Quantity="props">
                                                    <v-text-field
                                                        v-model="props.item.Quantity"
                                                        v-on:focus="$event.target.select()"
                                                        type="number">
                                                    </v-text-field>
                                                </template>
                                                <template v-slot:item.File="props">
                                                    <v-file-input
                                                        prepend-icon="mdi-camera"
                                                        v-model="props.item.File"
                                                        v-on:focus="onColumnFocus($event, props.item, 'File')"
                                                        v-bind:label="`${ props.item.CustomShadeName } layout file`"
                                                        v-bind:disabled="props.item.Quantity == 0 || !props.item.Quantity"
                                                        v-bind:name="camelCase(props.item.CustomShadeName)"
                                                    ></v-file-input>
                                                    <span
                                                        class="caption red--text"
                                                        v-show="props.item.ValidationMessages.has('File')"> {{ props.item.ValidationMessages.get('File') }}
                                                    </span>
                                                </template>
                                            </v-data-table>
                                        </v-col>
                                        <v-col cols="12" md="4">
                                            <h6 class="text-h6 text-center d-sm-block d-md-none">Gel Lip & Cheek Tint</h6>
                                            <v-row class="flex-md-column">
                                                <v-col cols="12" sm="6" md="12">
                                                    <v-img src="@/assets/SwatchImages/GelTint1.jpg"></v-img>
                                                </v-col>
                                                <v-col cols="12" sm="6" md="12">
                                                    <v-img src="@/assets/SwatchImages/GelTint2.jpg"></v-img>
                                                </v-col>
                                            </v-row>
                                        </v-col>
                                    </v-row>
                                </div>
                                <div id="matte-selection" v-show="bundleHasMatteTint">
                                    <v-row justify="center" class="flex-column flex-column-reverse flex-md-row">
                                        <v-col cols="12" md="8">
                                            <h6 class="text-h6 text-center d-none d-md-block">Matte Powdery</h6>
                                            <v-row>
                                                <v-col v-if="remainingMatteTint != Infinity">
                                                    <span >{{ remainingMatteTint }} remaining</span>
                                                </v-col>
                                                <v-col>
                                                    <h5 class="text-h5" v-if="isNoSelectedBundle"> PHP {{ matteTotalAmount }} </h5>
                                                    <span> {{ matteTotal }} pcs</span>
                                                </v-col>
                                            </v-row>
                                            <v-row justify="center" class="flex-column flex-md-row">
                                                <v-col cols="12">
                                                    <v-data-table
                                                        v-bind:headers="shadeSelectorHeaders"
                                                        v-bind:items="matteTints"
                                                        v-bind:hide-default-footer="true"
                                                        v-bind:disable-sort="true">
                                                        <template v-slot:item.CustomShadeName="props">
                                                            <v-text-field
                                                                v-model="props.item.CustomShadeName"
                                                                v-on:focus="onColumnFocus($event, props.item, 'CustomShadeName')">
                                                            </v-text-field>
                                                            <span
                                                                class="caption red--text"
                                                                v-show="props.item.ValidationMessages.has('CustomShadeName')"> {{ props.item.ValidationMessages.get('CustomShadeName') }}
                                                            </span>
                                                        </template>
                                                        <template v-slot:item.Quantity="props">
                                                            <v-text-field
                                                                v-model="props.item.Quantity"
                                                                v-on:focus="$event.target.select()" type="number">
                                                            </v-text-field>
                                                        </template>
                                                        <template v-slot:item.File="props">
                                                            <v-file-input
                                                                prepend-icon="mdi-camera"
                                                                v-model="props.item.File"
                                                                v-on:focus="onColumnFocus($event, props.item, 'File')"
                                                                v-bind:label="`${ props.item.Name } layout file`"
                                                                v-bind:disabled="props.item.Quantity == 0 || !props.item.Quantity"
                                                                v-bind:name="camelCase(props.item.CustomShadeName)">
                                                            </v-file-input>
                                                            <span
                                                                class="caption red--text"
                                                                v-show="props.item.ValidationMessages.has('File')"> {{ props.item.ValidationMessages.get('File') }}
                                                            </span>
                                                        </template>
                                                    </v-data-table>
                                                </v-col>
                                            </v-row>
                                        </v-col>
                                        <v-col cols="12" md="4" align-self="center">
                                            <h6 class="text-h6 text-center d-sm-block d-md-none">Matte Powdery</h6>
                                            <v-row justify="center">
                                                <v-col sm="6" md="12">
                                                    <v-img src="@/assets/SwatchImages/MattePowdery.jpg"></v-img>
                                                </v-col>
                                            </v-row>
                                        </v-col>
                                    </v-row>
                                </div>
                            </div>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols=6 md="2">
                            <h3 class="h3">
                                Total Amount {{ gelTotalAmount + matteTotalAmount}}
                            </h3>
                        </v-col>
                        <v-col cols=6 md="2" class="text-right">
                            <v-btn class="purple lighten-4"
                                rounded
                                v-bind:loading="isSubmitLoading" 
                                v-on:click="executeSubmitOrder">
                                Submit
                            </v-btn>
                        </v-col>
                    </v-row>
                </v-col>
            </v-row>
        </v-form>

        <!-- dialogs --> 
        <v-dialog id="no-bundle-limit-error" v-model="hasNoBundleLimitsError" v-on:click:outside="hasNoBundleLimitsError = false" min-width="290" max-width="400">
            <v-card>
                <v-card-title class="headline">Bundle Error</v-card-title>
                <v-card-text class="body-1">
                You have not selected enough items.
                
                Please make sure you have selected items worth more than <strong>PHP 1000</strong>.
                </v-card-text>
                <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="green darken-1" text v-on:click="hasNoBundleLimitsError = false">
                    Ok
                </v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <v-dialog id="bundle-limit-error" v-model="hasBundleLimitsError"  v-on:click:outside="hasBundleLimitsError = false" min-width="290" max-width="400">
            <v-card>
                <v-card-title class="headline">Bundle Error</v-card-title>
                <v-card-text class="body-1">
                    You might have selected less or more number of items for your bundle.
                    
                    Please fix this error and submit again.
                </v-card-text>
                <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="green darken-1" text v-on:click="hasBundleLimitsError = false">
                    Ok
                </v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <v-dialog id="order-submit-success"
            persistent
            v-model="isSuccessSubmitOrder"
            min-width="290" max-width="400">
            <v-card>
                <v-card-title class="headline">Order Submitted</v-card-title>
                <v-card-text class="body-1 text-center">

                    Your order is submitted. Please copy or screenshot this code.<br><br>
                    <h2 class="text-h2"> {{ orderUniqueCode }} </h2> <br>
                    You will use this to submit your payment/s.
                    <br><br>
                </v-card-text>
                <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="green darken-1" text v-on:click="dismissSuccessDialog">
                    Ok
                </v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </v-container>
</template>

<script>
import _ from 'lodash';
import $ from 'jquery';

const noBundle = {
    Code : "Custom",
    Description : "You choose your tints. Minimum of PHP 1000"
};

export default {
    data : function(){
        return {
            // details data
            clientName : '',
            fbName : '',
            shippingAddress : '',
            phoneNumber : '',
            brandName : '',
            countryCode: null,
            selectedBundle : noBundle,
            bundles : [],
            gelTints : [],
            matteTints : [],
            shadeSelectorHeaders : [
                {
                    text : 'Shade',
                    value : 'Name',
                },
                {
                    text : 'Your Shade Name',
                    value : 'CustomShadeName'
                },
                {
                    text : 'Quantity',
                    value : 'Quantity'
                },
                {
                    text : 'Layout Image',
                    value : 'File'
                }
            ],
            tintTypes: [],
            baseDetailsRules: [ v => !!v || 'This is required.' ],
            phoneNumberRules: [
                v => !!v || 'This is required.',
                v => v[0] == "9" || 'Invalid Phone number format. Please input as e.g. 977xxxxxxx',
                v => v.length == 10 || 'Phone number should have 10 digits. Please input as e.g. 977xxxxxxx'
            ],
            hasBundleLimitsError : false,
            hasNoBundleLimitsError : false,
            orderUniqueCode : '',
            isSuccessSubmitOrder : false,
            isSubmitLoading: false,
            initialGelTints: [],
            initialMatteTints: []
        }
    },
    methods : {
        selectBundle(bundle){
            this.selectedBundle = bundle;
            if(this.selectedBundle.Code == 'Custom'){
                return;
            }

            if(this.selectedBundle.BundleTintTypes.length == 2){
                return;
            }else{
                if(!this.bundleHasGelTint){
                    this.gelTints  = this.gelTints.map(this.resetShadeRow)
                }else if(!this.bundleHasMatteTint){
                    this.matteTints  = this.matteTints.map(this.resetShadeRow)
                }
            }
        },
        resetShadeRow(shade){
            const resetShade = Object.assign({}, shade);
            resetShade.Quantity = 0;
            resetShade.CustomShadeName = resetShade.Name;

            return resetShade;
        },
        onColumnFocus(e, shade, columnName) {
            e.target.select();
            if(shade.ValidationMessages.has(columnName)){
                const mapProxy = new Map(shade.ValidationMessages);
                mapProxy.delete(columnName);

                shade.ValidationMessages = mapProxy;
            }
        },
        countrySelected(val) {
            this.countryCode = val.dialCode;
        },
        loadResources(){
            const that = this;
            this.$axios({
                url : "api/customer-order/get-order-form-resources",
                crossdomain: false
            }).then(res => {
                that.bundles = [noBundle, ...res.data.Bundles];

                that.gelTints = res.data.GelTints.map(gelTint => {
                    return Object.assign({
                        CustomShadeName : gelTint.Name,
                        Quantity : 0,
                        File : null,
                        ValidationMessages : new Map()
                    }, gelTint);
                });

                that.matteTints = res.data.MatteTints.map(matteTint => {
                    return Object.assign({
                        CustomShadeName : matteTint.Name,
                        Quantity : 0,
                        File : null,
                        ValidationMessages : new Map()
                    }, matteTint);
                });

                that.tintTypes = res.data.TintTypes;
            });
        },
        async executeSubmitOrder(){
            this.isSubmitLoading = true;
            try {
                await this.submitOrder();
            }
            finally {
                this.isSubmitLoading = false;
            }
        },
        async submitOrder(){
            const formData = new FormData();
            const allTints = [...this.gelTints, ...this.matteTints];
            const selectedTints = allTints.filter(t => isShadeSelected(t));
            const formIsValid = this.$refs.form.validate();
            if(!formIsValid){
                const el = document.querySelector("#details-form");
                this.scrollToElement(el);
                return;
            }

            // validate shade rows
            const isShadesValid = this.isValidShades(selectedTints);
            if(!isShadesValid){
                this.$nextTick(() => {
                    const el = $("span.caption.red--text:visible");
                    this.scrollToElement(el[0]);
                });

                return;
            }

            // check bundle limits
            // validate selected bundle
            if(this.selectedBundle.Code == "Custom"){
                if(this.gelTotalAmount + this.matteTotalAmount < 1000){
                    this.hasNoBundleLimitsError = true;
                    return;
                }
            }else{
                if(this.remainingGelTint != 0 || this.remainingMatteTint != 0){
                    this.hasBundleLimitsError = true;
                    return;
                }
            }

            // check if custom shade names are unchanged
            // return if save on warning flag is not true
            const unchangedNames = selectedTints.filter(tint => tint.CustomShadeName == tint.Name)
                                                .map(tint => tint.Name);
            const hasUnchangedShadeName = !!unchangedNames.length;
            const unchangedNameHTML = unchangedNames.reduce((prev, curr) => `${prev}<strong>${curr}</strong><br>`, '');
            if(hasUnchangedShadeName){
                let saveOnWarning = await this.showHasUnchangedNamesWarning(unchangedNameHTML);
                if(!saveOnWarning.isConfirmed){
                    return Promise.resolve();
                }
            }

            const customerOrderShades = this.mapSelectedTints(selectedTints);
            selectedTints.forEach(t => formData.append(this.camelCase(t.CustomShadeName), t.File));

            const uniqueCode = generateUniqueCode(6);
            const orderDetails = {
                ClientName : this.clientName,
                ClientFacebookName : this.fbName,
                BundleCode : this.isNoSelectedBundle ? null : this.selectedBundle.Code,
                Amount : this.isNoSelectedBundle
                        ? this.gelTotalAmount + this.matteTotalAmount
                        : this.selectedBundle.Amount,
                BrandName : this.brandName,
                PhoneNumber : this.phoneNumber,
                DeliveryAddress : this.shippingAddress,
                UniqueCode : uniqueCode,
                CustomerOrderShades : customerOrderShades
            };

            formData.append('customerOrder', JSON.stringify(orderDetails));
            const that = this;
            return this.$axios({
                method : 'post',
                url : 'api/customer-order/submit',
                data : formData,
                headers: { 'content-type': 'multipart/form-data' },
                crossdomain: false
            }).then(res => {
                that.orderUniqueCode = res.data;
                that.isSuccessSubmitOrder = true;
            });
        },
        async showHasUnchangedNamesWarning(unchangedNames){
            return this.$swal.fire({
                title: 'Unchanged Shade Name',
                header: 'Unchanged Shade Name',
                html: `
                The following shades has unchanged shade name.
                <br><br>
                ${unchangedNames}
                <br><br>
                Do you wish to continue?
                `,
                customClass: {
                    content: 'sa-body-text',
                    actions: 'sa-body-text',
                    title: 'sa-body-heading',
                    header: 'sa-body-heading',
                },
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes',
                cancelButtonText: 'No'
            })
        },
        dismissSuccessDialog(){
            this.isSuccessSubmitOrder = false;
            this.$nextTick(() => {
                this.$router.go(0);
            })
        },
        clearForm(){
            this.$refs.form.reset();
        },
        isValidShades(customerOrderShades){
            this.validateShades(customerOrderShades);
            return !customerOrderShades.some(shade => shade.ValidationMessages.size);
        },
        validateShades(customerOrderShades){
            customerOrderShades.forEach(shade => {
                const isGel = shade.TintTypeId == 1;
                const validationMap = new Map();
                let targetShade;
                if(isGel){
                    targetShade = this.gelTints.find(gt => gt.Id == shade.Id);
                }else{
                    targetShade = this.matteTints.find(mt => mt.Id == shade.Id);
                }

                if(!targetShade.CustomShadeName){
                    validationMap.set('CustomShadeName', "Shade name can't be empty");
                }

                if(!targetShade.File){
                    validationMap.set('File', "Layout image can't be empty.");
                }

                targetShade.ValidationMessages = shade.ValidationMessages = validationMap;
            });
        },
        camelCase(str){
            return _.camelCase(str)
        },
        mapSelectedTints(tints){
            return tints
                    .filter(gt => parseInt(gt.Quantity) != 0 && !isNaN(parseInt(gt.Quantity)))
                    .map(gt => {
                        return {
                            ShadeId : gt.Id,
                            Quantity : gt.Quantity,
                            ShadeName : gt.CustomShadeName,
                            FileName : this.camelCase(gt.CustomShadeName)
                        }
                    });
        },
        onFocusShadeName(e, shade){
            e.target.select();
            if(shade.ValidationMessages.has('CustomShadeName')){
                shade.ValidationMessages.delete('CustomShadeName');
            }
        },
        scrollToElement(el){
            this.$nextTick(() => {
                el.scrollIntoView({
                    behavior : "smooth",
                    block : "center"
                });
            })
        }
    },
    computed: {
        remainingGelTint: function(){
            if(this.isNoSelectedBundle){
                return Infinity;
            }

            const bundleTintType = this.selectedBundle.BundleTintTypes.find(x => x.TintTypeId == 1);
            if(!bundleTintType){
                return 0;
            }

            return bundleTintType.Quantity - this.gelTotal;
        },
        remainingMatteTint: function(){
            if(this.isNoSelectedBundle){
                return Infinity;
            }

            const bundleTintType = this.selectedBundle.BundleTintTypes.find(x => x.TintTypeId == 2);
            if(!bundleTintType){
                return 0;
            }

            return bundleTintType.Quantity - this.matteTotal;
        },
        bundleHasGelTint: function(){
            if(!this.selectedBundle){
                return false;
            }

            if(this.isNoSelectedBundle){
                return true;
            }

            return this.selectedBundle.BundleTintTypes && this.selectedBundle.BundleTintTypes.map(btt => btt.TintTypeId).includes(1);
        },
        bundleHasMatteTint: function(){
            if(!this.selectedBundle){
                return false;
            }

            if(this.isNoSelectedBundle){
                return true;
            }

            return this.selectedBundle.BundleTintTypes && this.selectedBundle.BundleTintTypes.map(btt => btt.TintTypeId).includes(2);
        },
        isNoSelectedBundle: function(){
            if(!this.selectedBundle){
                return true;
            }

            return this.selectedBundle.Code == "Custom";
        },
        gelTotal: function(){
            return this.gelTints
                   ? this.gelTints.reduce((prev, curr) => prev + parseInt(curr.Quantity || 0), 0)
                   : [];
        },
        matteTotal: function(){
            return this.matteTints
                   ? this.matteTints.reduce((prev, curr) => prev + parseInt(curr.Quantity || 0), 0)
                   : [];
        },
        gelTotalAmount: function(){
            if(!this.tintTypes.length){
                return 0;
            }
            
            const gelTintUnitAmount = this.tintTypes.find(tt => tt.Id == 1).Price;
            return gelTintUnitAmount * this.gelTotal;
        },
        matteTotalAmount: function(){
            if(!this.tintTypes.length){
                return 0;
            }

            const matteUnitAmount = this.tintTypes.find(tt => tt.Id == 2).Price;
            return matteUnitAmount * this.matteTotal;
        }
    },
    beforeMount(){
        this.loadResources();
    }
}

function generateUniqueCode(length) {
   var result           = '';
   var characters       = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
   var charactersLength = characters.length;
   for ( var i = 0; i < length; i++ ) {
      result += characters.charAt(Math.floor(Math.random() * charactersLength));
   }
   return result;
}

function isShadeSelected(shade){
    return parseInt(shade.Quantity) != 0 && !isNaN(parseInt(shade.Quantity))
}

$(window).on('beforeunload', function() {
    $(window).scrollTop(0);
});

history.scrollRestoration = "manual"
</script>