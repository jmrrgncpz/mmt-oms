<template>
    <v-container>
        <v-row v-if="!!customerOrderDetails" class="flex-row" justify="start">
            <v-col cols="3">
                <v-card shaped>
                    <v-card-title>PHP {{ customerOrderDetails.amount }} </v-card-title>
                    <v-card-subtitle>Settlement Amount</v-card-subtitle>
                </v-card>
            </v-col>
            <v-col cols="3">
                <v-card shaped>
                    <v-card-title>PHP {{ remainingBalance }} </v-card-title>
                    <v-card-subtitle>Remaining Balance</v-card-subtitle>
                </v-card>
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="3">
                <h4 class="text-h4">Settlements</h4>
                <v-row class="flex-column">
                    <v-col>
                        <v-btn
                            class="purple--text"
                            v-if="remainingBalance > 0"
                            v-on:click="setupNewSettlementDialog"
                            outlined rounded>New Settlement</v-btn>
                    </v-col>
                    <v-col v-for="settlement in settlements" v-bind:key="settlement.id">
                        <v-card tile>
                            <v-card-title> PHP {{ settlement.amount }} </v-card-title>
                            <v-card-subtitle> {{ settlement.settlementDate }} </v-card-subtitle>
                        </v-card>
                    </v-col>
                </v-row>
            </v-col>
            <v-col cols="9">
                <h4 class="text-h4">Payment Images</h4>
                <v-row>
                    <v-col cols="4" v-for="paymentImage in paymentsImages" v-bind:key="paymentImage.id">
                        <v-card class="mx-auto" max-width="400">
                            <v-img class="white--text align-end" v-bind:src="`${paymentImage.paymentImageFilePath}`"></v-img>
                        </v-card>
                    </v-col>
                </v-row>
            </v-col>
        </v-row>
    
        <v-dialog v-model="isNewSettlementDialogVisible" max-width="350px" persistent>
            <v-card>
                <v-card-title>
                <span class="headline">New Settlement</span>
                </v-card-title>
                <v-container>
                    <v-row class="flex-column">
                        <v-col>
                            <v-text-field
                                type="number"
                                label="Amount"
                                v-model="settlementAmount"
                                prepend-icon="mdi-cash">
                            </v-text-field>
                        </v-col>
                        <v-col>
                            <v-menu
                                v-model="isDatePickerVisible"
                                v-bind:close-on-content-click="false"
                                v-bind:nudge-right="40"
                                transition="scale-transition"
                                offset-y
                                min-width="290px">
                                <template v-slot:activator="{ on, attrs }">
                                    <v-text-field
                                        v-model="settlementDate"
                                        label="Settlement Date"
                                        prepend-icon="mdi-calendar"
                                        readonly
                                        v-bind="attrs"
                                        v-on="on">
                                    </v-text-field>
                                </template>
                                <v-date-picker v-model="settlementDate" v-on:input="isDatePickerVisible = false"></v-date-picker>
                            </v-menu>
                        </v-col>
                    </v-row>
                </v-container>
                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn color="blue darken-1" text v-on:click="isNewSettlementDialogVisible = false">Close</v-btn>
                    <v-btn
                        color="blue darken-1"
                        text
                        v-on:click="submitNewSettlement"
                        v-bind:loading="isSubmittingSettlement">
                            Save
                    </v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </v-container>
</template>

<script>
export default{
    data(){
        return{
            paymentsImages: [],
            settlements: [],
            isNewSettlementDialogVisible: false,
            // new settlement
            settlementDate: '',
            settlementAmount: 0,
            isDatePickerVisible: false,
            isSubmittingSettlement: false
        }
    },
    props: ['customerOrderDetails'],
    methods: {
        submitNewSettlement(){
            this.isSubmittingSettlement = true;
            const that = this;
            this.$axios({
                method: 'post',
                crossdomain: false,
                url: 'api/customer-order/create-settlement',
                data: {
                    SettlementDate: that.settlementDate,
                    Amount: that.settlementAmount,
                    CustomerOrderId: that.$route.params.id
                }
            })
            .then(res => {
                const newSettlement = {
                    id: res.data.Id,
                    amount: res.data.Amount,
                    settlementDate: res.data.SettlementDate
                };

                const newSettlements = [newSettlement, ...that.settlements];
                that.settlements = newSettlements;
                that.isNewSettlementDialogVisible = false;
            })
            .finally(() => {
                that.isSubmittingSettlement = false;
            });
        },
        setupNewSettlementDialog(){
            this.settlementAmount = this.remainingBalance;
            this.isNewSettlementDialogVisible = true;
        }
    },
    setRemainingBalance(){

    },
    computed: {
        remainingBalance(){
            return this.customerOrderDetails.amount - this.settlements.reduce((prev, curr) => prev + curr.amount, 0) ;
        }
    },
    mounted(){
        const that = this;
        
        this.$axios({
            method : 'get',
            crossdomain: false,
            url: `api/customer-order/${that.$route.params.id}/payments`
        })
        .then(res => {
            that.paymentsImages = res.data.map(p => {
                return {
                    id: p.Id,
                    customerOrderId: p.CustomerOrderId,
                    paymentImageFilePath: p.PaymentImageFilePath
                }
            });
        });

        this.$axios({
            method: 'get',
            crossdomain: false,
            url: `api/customer-order/${that.$route.params.id}/settlements`
        })
        .then(res => {
            that.settlements = res.data.map(s => {
                return {
                    id: s.Id,
                    amount: s.Amount,
                    settlementDate: s.SettlementDate
                }
            });
        });

        this.settlementDate = new Date().toISOString().substr(0, 10);
    }
}
</script>