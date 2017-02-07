from django.http import HttpResponse

def index(request):
    return HttpResponse("<h2> ForexFactory Current Week High Impact USD Event")